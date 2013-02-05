module Api
	module V1
		class ComputersController  < ApplicationController
			respond_to :json
			def index
				token = params[:auth_token]
				if token.nil?
					render :status=>400,
		              :json=>{:message=>"The request must contain a token."}
		       		return
		       	end
				@user = User.find_by_authentication_token(token)
				render :status=>200, :json=>{:user=>@user, :computers=>@user.computer}
				return
			end

			def create
				token = params[:auth_token]
				name = params[:name]
				enviroment = params[:enviroment]
				ip = params[:ip_address]

				if token.nil?
					render :status=>400,
		              :json=>{:message=>"The request must contain an auth token."}
		       		return
		       	end

		       	if name.nil?
		       		render :status=>400,
		              :json=>{:message=>"The request must contain a computer name."}
		       		return
		       	end
		       	if enviroment.nil?
		       		render :status=>400,
		              :json=>{:message=>"The request must contain a computer enviroment."}
		       		return
		       	end

		       	@computer = Computer.new(:enviroment => enviroment, :name => name, :ip_address => ip)

				@user = User.find_by_authentication_token(token)
				@computer.user_id = @user.id
				@computer.save

				render :status=>200, :json=>{:computer=>@computer}
				return
			end

			def destroy

			end


		end
	end
end