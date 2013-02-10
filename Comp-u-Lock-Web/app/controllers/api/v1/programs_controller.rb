module Api
  module V1
  	class ProgramsController < ApplicationController
  		before_filter :authenticate_user!
		respond_to :json

		def create
			token = params[:auth_token]
			program = JSON.parse params[:program]
			if token.nil?
				render :status => 400,
					:json => { :message => "The request must contain an auth token."}
				return
			end
			account_id = program["account_id"]
			if account_id.nil?
				render :status => 400,
					:json => { :message => "The request must contain an account id."}
				return
			end

			if program["name"].nil?
				render :status => 400,
					:json => { :message => "The request must contain a name."}
				return
			end

			@user = User.find_by_authentication_token token
			@accounts = Account.where computer_id: @user.computer_ids

			unless @accounts.pluck(:id).include? account_id
				render :status => 401,
					:json => { :message => "The request was declined. Check account_id."}
				return
			end
			@program = AccountProgram.new(program)
			if @program.save
				render json: {program: @program}
			else
				render :status => 400,
					:json => { :message => "There was a problem saving the entity."}
				return
			end
		end

		def show
			token = params[:auth_token]
			program_id = params[:id]
			if token.nil?
				render :status => 400,
					:json => { :message => "The request must contain an auth token."}
				return
			end
			if program_id.nil? 
				render :status => 400,
					:json => { :message => "The request must contain a program id."}
				return
			end
			@user = User.find_by_authentication_token token
			@program = Accountprogram.find program_id
			@accounts = Account.where computer_id: @user.computer_ids

			if @accounts.pluck(:id).include? @program.account_id
				render json: {program: @program}
			else
				render :status => 401,
					:json => { :message => "The request was declined. Check Account Id."}
				return
			end
		end

		def index

		end
  	end
  end
end