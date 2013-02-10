module Api
  module V1
  	class ProcessesController < ApplicationController
  		before_filter :authenticate_user!
		respond_to :json

		def create
			token = params[:auth_token]
			if token.nil?
				render :status => 400,
					:json => { :message => "The request must contain an auth token."}
				return
			end
		end

		def show

		end

		def index

		end
  	end
  end
end