module Api
	module V1
		class ComputersController  < ApplicationController
			before_filter :authenticate_user!
			respond_to :json

			def index
				token = params[:auth_token]

				if token.nil?
					render :status=>400,
		              :json=>{:message=>"The request must contain a token."}
		       		return false
		       	end
				@user = User.find_by_authentication_token token
				if @user.nil?
					render :status=>401,
		              :json=>{:message=>"Authentication Error, Check token."}
		       		return
		       	end
					render :json => {computers: @user.computer}
				return
			end

			def create
				token = params[:auth_token]
				computer = JSON.parse params[:computer]

				if token.nil?
					render :status=>400,
		              :json=>{:message=>"The request must contain a token."}
		       		return false
		       	end
		       	if computer["name"].nil?
		       		render :status=>400,
		              :json=>{:message=>"The request must contain a computer name."}
		       		return
		       	end
		       	if computer["enviroment"].nil?
		       		render :status=>400,
		              :json=>{:message=>"The request must contain a computer enviroment."}
		       		return
		       	end

		       	@computer = Computer.new(computer)

				@user = User.find_by_authentication_token(token)
				@computer.user_id = @user.id
				@computer.save

				render :json=>{:computer=>@computer}
				return
			end

			def update
				token = params[:auth_token]
				id = params[:id]
				computer = JSON.parse params[:computer]

				if token.nil?
					render :status=>400,
		              :json=>{:message=>"The request must contain a token."}
		       		return false
		       	end

				@user = User.find_by_authentication_token token

				if computer["name"].nil?
		       		render :status=>400,
		              :json=>{:message=>"The request must contain a computer name."}
		       		return
		       	end
		       	if computer["enviroment"].nil?
		       		render :status=>400,
		              :json=>{:message=>"The request must contain a computer enviroment."}
		       		return
		       	end

		       	unless @user.computer_ids.include? id.to_i
					render :status => 401,
						:json => { :message => "The request was declined. Check computer Id."}
					return
				end
		       	if Computer.update(id, computer)
		       		@computer = Computer.find(id)
		       		render json: {computer: @computer}
		       	else
		       		render :status => 400,
						:json => { :message => "Something went wrong with saving the entity."}
					return
		       	end
			end

			def show
				token = params[:auth_token]
				id = params[:id]

				if token.nil?
					render :status => 400,
						:json => { :message => "The request must contain an auth token."}
					return
				end
				if id.nil?
					render :status => 400,
						:json => { :message => "The request must contain an id."}
					return
				end

				@user = User.find_by_authentication_token(token)
				unless @user.computer_ids.include? id.to_i
					render :status => 401,
						:json => { :message => "The request was declined. Check computer Id."}
					return
				end

				@computer = Computer.find(id)
				respond_to do |format|
					format.json {render json: @computer}
				end
			end

			def destroy
				token = params[:auth_token]
				id = params[:id]

				if token.nil?
					render :status => 400,
						:json => { :message => "The request must contain an auth token."}
					return
				end
				if id.nil?
					render :status => 400,
						:json => { :message => "The request must contain an id."}
					return
				end
				@user = User.find_by_authentication_token(token)
				unless @user.computer_ids.include? id.to_i
					render :status => 401,
						:json => { :message => "The request was declined. Check computer Id."}
					return
				end

				if Computer.find(id).delete
					respond_to do |format|
						format.json { render :json => {message: "Success"} }
					end
				else
					render :status => 400,
						:json => { :message => "Something went wrong with deleting the entity."}
					return
				end
			end
		end
	end
end